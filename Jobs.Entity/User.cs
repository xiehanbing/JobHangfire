using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jobs.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("User")]
    public class User
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string  UserId { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string  Account { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string  Password { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
