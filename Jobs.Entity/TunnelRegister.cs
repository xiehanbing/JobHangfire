using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jobs.Entity
{
    [Table("TunnelRegister")]
    public class TunnelRegister
    {
        /// <summary>
        /// 自增主键
        /// </summary>		
        [Key]
        public int No { get; set; }
        /// <summary>
        /// 标题
        /// </summary>		
        public string Title { get; set; }
        /// <summary>
        /// 业主单位
        /// </summary>		
        public string OwnerUnit { get; set; }
        /// <summary>
        /// 业主单位负责人
        /// </summary>		
        public string OwnerContact { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>		
        public string WorkUnit { get; set; }
        /// <summary>
        /// 作业单位负责人
        /// </summary>		
        public string WorkContact { get; set; }
        /// <summary>
        /// 作业单位负责人手机
        /// </summary>		
        public string WorkPhone { get; set; }
        /// <summary>
        /// 区域编号
        /// </summary>		
        public string AreaNumber { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>		
        public string WorkDetail { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>		
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// 终止时间
        /// </summary>		
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 是否用电
        /// </summary>		
        public int? IsElectricity { get; set; }
        /// <summary>
        /// 是否用火
        /// </summary>		
        public int? IsFire { get; set; }
        /// <summary>
        /// 要求配合条件
        /// </summary>		
        public string CoordinateCondition { get; set; }
        /// <summary>
        /// 附件
        /// </summary>		
        public string AnnexAddress { get; set; }
        /// <summary>
        /// 是否同意
        /// </summary>		
        public int? IsConfirm { get; set; }
        /// <summary>
        /// 作业人姓名
        /// </summary>		
        public string WorkUserName { get; set; }
        /// <summary>
        /// 作业人电话
        /// </summary>		
        public string WorkUserPhone { get; set; }
        /// <summary>
        /// 当前入廊申请状态
        /// </summary>		
        public byte? Status { get; set; }
        /// <summary>
        /// IsDeleted
        /// </summary>		
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 创建人工号
        /// </summary>		
        public string CreateNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? CreationTime { get; set; }
        /// <summary>
        /// ApproveStatus
        /// </summary>		
        public byte ApproveStatus { get; set; }
        /// <summary>
        /// LastModifyTime
        /// </summary>		
        public DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 备注、拒绝理由
        /// </summary>		
        public string Comment { get; set; }
        /// <summary>
        /// PostBackFileId
        /// </summary>		
        public string PostBackFileId { get; set; }
        /// <summary>
        /// CreateRoleNo
        /// </summary>		
        public int CreateRoleNo { get; set; }
    }
}