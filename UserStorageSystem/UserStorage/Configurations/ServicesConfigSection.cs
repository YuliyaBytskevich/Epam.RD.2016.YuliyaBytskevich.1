﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserStorage.Configurations
{
    public class ServicesConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Services", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServicesCollection), AddItemName = "add")]
        public ServicesCollection SectionItems
        {
            get { return ((ServicesCollection)(this["Services"])); }
        }


        public ServicesCollection ServicesCollection
        {
            get
            {
                object o = this["Services"];
                return o as ServicesCollection;
            }
        }

        public static ServicesConfigSection GetConfig()
        {
            return (ServicesConfigSection)System.Configuration.ConfigurationManager.GetSection("ServicesSection") ?? new ServicesConfigSection();
        }

    }
}