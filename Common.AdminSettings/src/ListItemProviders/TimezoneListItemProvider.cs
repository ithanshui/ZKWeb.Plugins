﻿using System;
using System.Collections.Generic;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.Common.AdminSettings.src.ListItemProviders {
	/// <summary>
	/// 时区列表
	/// </summary>
	public class TimezoneListItemProvider : IListItemProvider {
		/// <summary>
		/// 获取选项列表
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItem> GetItems() {
			foreach (var zone in TimeZoneInfo.GetSystemTimeZones()) {
				yield return new ListItem(zone.DisplayName, zone.StandardName);
			}
		}
	}
}
