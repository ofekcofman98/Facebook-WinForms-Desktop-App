using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BasicFacebookFeatures
{
    public static class Singleton<T>
        where T : class
    {
        private static volatile T s_Instance;

        private static object s_LockObj = new object();
        static Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_LockObj)
                    {
                        if (s_Instance == null)
                        {
                            Type typeOfT = typeof(T);
                            ConstructorInfo constructor = typeof(T).GetConstructor(
                                BindingFlags.Instance | BindingFlags.NonPublic,
                                null,
                                Type.EmptyTypes,
                                null);





                            if (constructor != null)
                            {
                                s_Instance = (T)constructor.Invoke(null);
                            }

                        }
                    }
                }

                return s_Instance;
            }
        }
    }
}
