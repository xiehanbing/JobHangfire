# JobHangfire
jobserver


Startup:
```
  //注入hangfire 服务
            services.AddHangfire(x =>
            {
                x.UseSqlServerStorage(Configuration.GetConnectionString(ApiConfig.JobDbConnectionName));
                x.UseRecurringJob(new JobProvider());
                x.UseConsole();
                x.UseFilter(new LogEverythingAttribute());
                x.UseHeartbeatPage(checkInterval: TimeSpan.FromSeconds(1));
                x.UseDashboardRecurringJobExtensions();
            });
            ApiConfig.HangfireLogUrl = Configuration["HangfireLogFileUrl"];
 ```
JobProvider:
```
   public class JobProvider : IConfigurationProvider
    {
        public IEnumerable<RecurringJobInfo> Load()
        {
            var result = new List<RecurringJobInfo>();
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null) return result;
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetTypeInfo().DeclaredMethods)
                {
                    //if(!method.IsDefined(typeof(RecurringJobInfo),false))
                    //    continue;
                    var attribute = method.GetCustomAttribute<RecurringJobAttribute>(false);
                    if (attribute == null) continue;
                    if (string.IsNullOrWhiteSpace(attribute.RecurringJobId))
                    {
                        attribute.RecurringJobId =method.GetRecurringJobId();
                    }
                    result.Add(new RecurringJobInfo()
                    {
                        RecurringJobId = attribute.RecurringJobId,
                        Cron = attribute.Cron,
                        Queue = attribute.Queue,
                        Enable = attribute.Enabled,
                        Method = method,
                        TimeZone = TimeZoneInfo.Local
                    });
                }
            }

            return result;
        }
       
    }
    ```
   JobExtension:
   ```
    public static class JobExtension
    {
        /// <summary>
        /// GetRecurringJobId
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetRecurringJobId(this MethodInfo method)
        {
            return method.DeclaringType.ToGenericTypeString() + "." + method.Name;
        }
        public static string ToGenericTypeString(this Type type)
        {
            if (!type.GetTypeInfo().IsGenericType)
            {
                return type.GetFullNameWithoutNamespace()
                        .ReplacePlusWithDotInNestedTypeName();
            }

            return type.GetGenericTypeDefinition()
                    .GetFullNameWithoutNamespace()
                    .ReplacePlusWithDotInNestedTypeName()
                    .ReplaceGenericParametersInGenericTypeName(type);
        }
        public static Type[] GetAllGenericArguments(this TypeInfo type)
        {
            return type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments : type.GenericTypeParameters;
        }

        private static bool TypesMatchRecursive(TypeInfo parameterType, TypeInfo actualType, IList<Type> genericArguments)
        {
            if (parameterType.IsGenericParameter)
            {
                var position = parameterType.GenericParameterPosition;

                // Return false if this generic parameter has been identified and it's not the same as actual type
                if (genericArguments[position] != null && genericArguments[position].GetTypeInfo() != actualType)
                {
                    return false;
                }

                genericArguments[position] = actualType.AsType();
                return true;
            }

            if (parameterType.ContainsGenericParameters)
            {
                if (parameterType.IsArray)
                {
                    // Return false if parameterType is array whereas actualType isn't
                    if (!actualType.IsArray) return false;

                    var parameterElementType = parameterType.GetElementType();
                    var actualElementType = actualType.GetElementType();

                    return TypesMatchRecursive(parameterElementType.GetTypeInfo(), actualElementType.GetTypeInfo(), genericArguments);
                }

                if (!actualType.IsGenericType || parameterType.GetGenericTypeDefinition() != actualType.GetGenericTypeDefinition())
                {
                    return false;
                }

                for (var i = 0; i < parameterType.GenericTypeArguments.Length; i++)
                {
                    var parameterGenericArgument = parameterType.GenericTypeArguments[i];
                    var actualGenericArgument = actualType.GenericTypeArguments[i];

                    if (!TypesMatchRecursive(parameterGenericArgument.GetTypeInfo(), actualGenericArgument.GetTypeInfo(), genericArguments))
                    {
                        return false;
                    }
                }

                return true;
            }

            return parameterType == actualType;
        }

        private static string GetFullNameWithoutNamespace(this Type type)
        {
            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            const int dotLength = 1;
            // ReSharper disable once PossibleNullReferenceException
            return !String.IsNullOrEmpty(type.Namespace)
                ? type.FullName.Substring(type.Namespace.Length + dotLength)
                : type.FullName;
        }

        private static string ReplacePlusWithDotInNestedTypeName(this string typeName)
        {
            return typeName.Replace('+', '.');
        }

        private static string ReplaceGenericParametersInGenericTypeName(this string typeName, Type type)
        {
            var genericArguments = type.GetTypeInfo().GetAllGenericArguments();

            const string regexForGenericArguments = @"`[1-9]\d*";

            var rgx = new Regex(regexForGenericArguments);

            typeName = rgx.Replace(typeName, match =>
            {
                var currentGenericArgumentNumbers = int.Parse(match.Value.Substring(1));
                var currentArguments = string.Join(",", genericArguments.Take(currentGenericArgumentNumbers).Select(ToGenericTypeString));
                genericArguments = genericArguments.Skip(currentGenericArgumentNumbers).ToArray();
                return string.Concat("<", currentArguments, ">");
            });

            return typeName;
        }
    }
    
    ```
    demo:
    ```
     /// <summary>
        /// 执行取消过期入廊申请 每1分钟执行一次
        /// </summary>
        [RecurringJob("0 0/1 * * * ?", queue: HangfireQueue.Job)]
        [JobDisplayName("执行取消过期入廊申请 每1分钟执行一次")]
        public void Cancel(PerformContext context)
        {
            context.WriteLine("执行取消过期入廊申请");

        }
      ```
      
