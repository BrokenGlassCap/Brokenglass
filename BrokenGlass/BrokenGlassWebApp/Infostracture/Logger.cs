using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace BrokenGlassWebApp
{
    public class ApplicationLogger
    {
        private static Logger m_logger;

        public static Logger Instance
        {
            get
            {
                if (m_logger == null)
                {
                    m_logger = LogManager.GetCurrentClassLogger();
                }
                return m_logger;
            }
        }
    }
}