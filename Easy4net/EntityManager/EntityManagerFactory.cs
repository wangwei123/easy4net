using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Easy4net.EntityManager
{
    public class EntityManagerFactory
    {
        public static EntityManager CreateEntityManager()
        {
            return new EntityManagerImpl();
        }
    }
}
