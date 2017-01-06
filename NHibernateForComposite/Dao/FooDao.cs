using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

namespace NHibernateForComposite.Dao
{
    public class FooDao
    {
        public ISession OpenSession()
        {
            return Core.NHbHelper.Session.OpenSession();
        }

        public void Create(Models.Foo foo)
        {
            var session = OpenSession();
            session.Save(foo);
            session.Flush();            
        }

        public void CreateForMerge(Models.Foo foo)
        {
            var session = OpenSession();
            session.Save(session.Merge(foo));
            session.Flush();
        }

        public IList<Models.Foo> GetList( string id, string groupNumber )
        {
            var session = OpenSession();
            ICriteria criteria = session.CreateCriteria(typeof(Models.Foo));
            IList<NHibernate.Criterion.ICriterion> lstCriterion = new List<NHibernate.Criterion.ICriterion>();
            lstCriterion.Add(Expression.Eq("BN.Id", id)); //对应的是实例类字体
            lstCriterion.Add(Expression.Eq("BN.GroupNumber", groupNumber)); //对应的是实例类字体

            foreach (var item in lstCriterion)
            {
                criteria.Add(item);
            }
            return criteria.List<Models.Foo>();
        }

        public Models.Foo GetFoo(string id, string groupNumber)
        {
            var session = OpenSession();
            return null;
        }
    }
}
