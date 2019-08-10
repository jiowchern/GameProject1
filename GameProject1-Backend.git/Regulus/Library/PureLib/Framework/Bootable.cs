﻿namespace Regulus.Framework
{
	public interface IBootable<T>
	{
		void Launch();

		void Shutdown();
	}

	/// <summary>
	///     啟動器
	/// </summary>
	public interface IBootable
	{
		void Launch();

		void Shutdown();
	}
}
