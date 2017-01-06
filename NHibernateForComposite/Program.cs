using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernateForComposite
{
    class Program
    {
        static Dao.FooDao dao = new Dao.FooDao();
        static void Main(string[] args)
        {

            InsertData();
            ListData();


            Console.ReadKey();
        }

        public static void ListData()
        {

            var ls = dao.GetList("000", "1").FirstOrDefault();
            foreach (var item in ls.Childs)
            {
                Console.WriteLine(item.BN.Id);
            }
        }

        static void InsertData()
        {

            Func<Models.Foo, IList<Models.Foo>> secondList = (parent) =>
            {
                List<Models.Foo> ls = new List<Models.Foo>();
                for (var irow = 10; irow < 15; ++irow)
                {
                    Models.BaseInfo key = new Models.BaseInfo()
                    {
                        Id = parent.BN.Id + "-00" + irow.ToString(),
                        GroupNumber = "1"
                    };

                    Models.Foo foo = new Models.Foo()
                    {
                        BN = key,
                        Content = parent.BN.Id + "-Content" + irow.ToString(),
                        Parent = parent
                    };
                    ls.Add(foo);
                }
                return ls;
            };

            for (var index = 0; index < 6; ++index)
            {
                Models.BaseInfo key = new Models.BaseInfo()
                {
                    Id = "00" + index.ToString(),
                    GroupNumber = "1"
                };

                Models.Foo foo = new Models.Foo()
                {
                    BN = key,
                    Content = "Content" + index.ToString(),
                    Parent = null,
                };
                foo.Childs = secondList(foo);

                dao.CreateForMerge(foo);
            }
        }
    }
}
