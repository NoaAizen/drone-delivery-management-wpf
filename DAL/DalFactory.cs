using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string type)
        {
            switch(type)
            {
                case "1":
                    return DalObject.DalObject.Instance;
                    //return new DalObject.DalObject();
                case "2":
                    return DalXml.DalXml.Instance;
                default:
                    //throw new...
                    return DalObject.DalObject.Instance;
            }
            
        }

    }
}
