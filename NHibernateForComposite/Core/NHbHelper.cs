using System;
using System.Collections.Generic; 
using System.Text;

namespace NHibernateForComposite.Core
{
    public static class NHbHelper
    {
        public static NHibernate.Cfg.Configuration _configuration;

        public static void InitConfiguration()
        {
            _configuration = new NHibernate.Cfg.Configuration();
            using (System.IO.MemoryStream _memery = new System.IO.MemoryStream())
            {
                NHibernate.Mapping.Attributes.HbmSerializer.Default.Serialize(_memery, typeof(Models.Foo));
                _memery.Position = 0;
                _configuration.AddInputStream(_memery);
            };
        }

        public static NHibernate.ISessionFactory Session
        {
            get
            {
                if (_configuration == null)
                {
                    InitConfiguration();
                }
                return _configuration.BuildSessionFactory();
            }
        }
    }
}
