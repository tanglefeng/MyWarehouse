using System;

namespace Kengic.Was.Application.WasModel.Dto
{
    public abstract class EntityDto<TKey> : EntityBaseDto<TKey>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public abstract class EntityBaseDto<TKey>
    {
        public TKey Id { get; set; }
    }


    public abstract class EntityForTimeDto<TKey> : EntityDto<TKey>
    {
        public string CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}