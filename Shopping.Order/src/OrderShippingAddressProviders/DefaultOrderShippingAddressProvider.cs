﻿using System.Collections.Generic;
using ZKWeb.Plugins.Shopping.Order.src.Database;
using ZKWeb.Plugins.Shopping.Order.src.Managers;
using ZKWeb.Plugins.Shopping.Order.src.Model;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Shopping.Order.src.OrderShippingAddressProviders {
	/// <summary>
	/// 默认的创建订单时可用的收货地址提供器
	/// </summary>
	[ExportMany]
	public class DefaultOrderShippingAddressProvider : IOrderShippingAddressProvider {
		/// <summary>
		/// 获取可用的收货地址
		/// 获取用户添加的收货地址
		/// </summary>
		public void GetShippingAddresses(long? userId, IList<UserShippingAddress> addresses) {
			// TODO: 下个版本改成AddRange
			var shippingAddressManager = Application.Ioc.Resolve<UserShippingAddressManager>();
			foreach (var address in shippingAddressManager.GetShippingAddresses(userId)) {
				addresses.Add(address);
			}
		}
	}
}
