﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace ScoopFramework.Mvc.Grid.Settings
{
    public class GridColumnSettings<T> : GridColumnSettings
     where T : class
    {
        public Action<T> Template
        {
            get;
            set;
        }
    }

    public class GridColumnSettings
    {
        private string member;
        private string clientTemplate;

        public GridColumnSettings()
        {
            Encoded = true;
            Visible = true;
            HtmlAttributes = new RouteValueDictionary();
        }

     

        public string ClientTemplate
        {
            get
            {
                return clientTemplate;
            }
            set
            {
                clientTemplate = HttpUtility.HtmlDecode(value);
            }
        }

       

        public bool Encoded
        {
            get;
            set;
        }

        public string Format
        {
            get;
            set;
        }

        public bool Hidden
        {
            get;
            set;
        }

        public IDictionary<string, object> HtmlAttributes
        {
            get;
            private set;
        }

        public string Member
        {
            get
            {
                return member;
            }
            set
            {
                member = value;

                if (!Title.HasValue())
                {
                    Title = member.AsTitle();
                }
            }
        }

        public Type MemberType
        {
            get;
            set;
        }


        public string Title
        {
            get;
            set;
        }

        public bool Visible
        {
            get;
            set;
        }

        public string Width
        {
            get;
            set;
        }
    }
}
