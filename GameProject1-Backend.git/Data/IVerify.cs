﻿using Regulus.Remote;

namespace Regulus.Project.GameProject1.Data
{
	/// <summary>
	///     驗證介面
	/// </summary>
	public interface IVerify
	{
		/// <summary>
		///     登入
		/// </summary>
		/// <param name="id">
		///     帳號.
		/// </param>
		/// <param name="password">
		///     密碼.
		/// </param>
		/// <returns>
		///     回傳真則為成功
		/// </returns>
		Value<bool> Login(string id, string password);
	}
}
