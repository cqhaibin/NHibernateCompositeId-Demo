using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace NHibernateForComposite.Models
{
    [Serializable]
    public class BaseInfo
    {
        public virtual string Id { get; set; }
        public virtual string GroupNumber { get; set; }

        public override bool Equals(object obj)
        {
            var baseInfo = obj as BaseInfo;
            if (baseInfo == null)
            {
                return false;
            }

            return baseInfo.Id == this.Id && baseInfo.GroupNumber == this.GroupNumber;
        }
        public override int GetHashCode()
        {
            return (this.Id + "|" + this.GroupNumber).GetHashCode(); //判断缓存是否存在，已此作为Key
        }
    }
    /// <summary>
    /// 如果直接继承BaseInfo类，会导致Nhibernate获取不到主键，而无法使用session.merage等方法
    /// 具体原因：都是assigned主键生成策略，但继承后主键生成类的IsEmbedded会为true，表示已嵌入主键
    /// 如果引用方式的联合主键，IsEmbedded会为false
    /// </summary>
    [Serializable]
    [Class(Table = "t_Foo")] 
    public class Foo
    {
        [CompositeId(0, Name = "BN")]
        [KeyProperty(1, Name = "Id", Column = "Id", TypeType = typeof(string))]
        [KeyProperty(2, Name = "GroupNumber", Column = "GroupNumber", TypeType = typeof(string))]
        public virtual BaseInfo BN { get; set; }

        [Property(Name = "Content", Column = "Content", TypeType = typeof(string))]
        public virtual string Content { get; set; }

        [Bag(0, Name = "Childs", Cascade = "all", Lazy = CollectionLazy.False, Inverse = true)]
        [Key(1)]
        [Column(2, Name = "ParentId")]
        [Column(3, Name = "GroupNumber")]
        [OneToMany(4, ClassType = typeof(Foo))]
        public virtual IList<Foo> Childs { get; set; }

        //联合主键实现ManyToOne，不建立实现insert和update的级联操作，因这样会引起ORM报错   Insert = false, Update = false,
        //[Property(Name = "ParentId", Column = "ParentId", TypeType = typeof(string))] //由于使用了联合主键实现ManyToOne，但禁用了insert和update级联操作，所以关联字段需要手动写入
        //public virtual string ParentId { get; set; }

        //外键与联合主键不要共用字段
        [ManyToOne(0, Name = "Parent", ClassType = typeof(Foo))] 
        [Column(1, Name = "ParentId")]
        [Column(2, Name = "ParentGroupNumber")]
        public virtual Foo Parent { get; set; }
    }
}
