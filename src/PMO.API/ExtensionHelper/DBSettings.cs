﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMO.API.ExtensionHelper
{
	public class DBSettings
	{
		public RavenDB RavenDb { get; set; }
	}
	public class RavenDB
	{
		public string[] Urls { get; set; }
		public string DatabaseName { get; set; }
		public string CertPath { get; set; }
		public string CertPass { get; set; }
	}
	
}