﻿using System;

namespace Regulus.Remote
{
    public delegate void InvokeEventCallabck(Guid entity_id , int event_id , object[] args);
    public class GenericEventClosure
	{
		private readonly Guid EntityId;

		private readonly int EventId;

		private readonly InvokeEventCallabck InvokeEvent;

		public GenericEventClosure(Guid entity_id, int event_id, InvokeEventCallabck invokeevent)
		{
			EntityId = entity_id;
			EventId = event_id;
			InvokeEvent = invokeevent;
		}

		public static Type GetDelegateType()
		{
			return typeof(Action);
		}

		public void Run()
		{
			InvokeEvent(
				EntityId, 
				EventId, 
				new object[]
				{
				});
		}
	}

    public class GenericEventClosure<T1>
	{
		private readonly Guid EntityId;

		private readonly int EventId;

		private readonly InvokeEventCallabck InvokeEvent;

		public GenericEventClosure(Guid entity_id, int event_id, InvokeEventCallabck invokeevent)
        {
			EntityId = entity_id;
			EventId = event_id;
			InvokeEvent = invokeevent;
		}

		public static Type GetDelegateType()
		{
			return typeof(Action<T1>);
		}

		public void Run(T1 arg1)
		{
			InvokeEvent(
				EntityId, 
				EventId, 
				new object[]
				{
					arg1
				});
		}
	}

    public class GenericEventClosure<T1, T2>
	{
		private readonly Guid EntityId;

		private readonly int _EventId;

		private readonly InvokeEventCallabck InvokeEvent;

        public GenericEventClosure(Guid entity_id, int event_id, InvokeEventCallabck invokeevent)
        {
			EntityId = entity_id;
			_EventId = event_id;
			InvokeEvent = invokeevent;
		}

		public static Type GetDelegateType()
		{
			return typeof(Action<T1, T2>);
		}

		public void Run(T1 arg1, T2 arg2)
		{
			InvokeEvent(
				EntityId, 
				_EventId, 
				new object[]
				{
					arg1, 
					arg2
				});
		}
	}

    public class GenericEventClosure<T1, T2, T3>
	{
		private readonly Guid EntityId;

		private readonly int EventId;

		private readonly InvokeEventCallabck InvokeEvent;

        public GenericEventClosure(Guid entity_id, int event_id, InvokeEventCallabck invokeevent)
        {
			EntityId = entity_id;
			EventId = event_id;
			InvokeEvent = invokeevent;
		}

		public static Type GetDelegateType()
		{
			return typeof(Action<T1, T2, T3>);
		}

		public void Run(T1 arg1, T2 arg2, T3 arg3)
		{
			InvokeEvent(
				EntityId, 
				EventId, 
				new object[]
				{
					arg1, 
					arg2, 
					arg3
				});
		}
	}

    public class GenericEventClosure<T1, T2, T3, T4>
	{
		private readonly Guid EntityId;

		private readonly int EventId;

		private readonly InvokeEventCallabck InvokeEvent;

		public GenericEventClosure(Guid entity_id, int event_id, InvokeEventCallabck invokeevent)
		{
			EntityId = entity_id;
			EventId = event_id;
			InvokeEvent = invokeevent;
		}

		public static Type GetDelegateType()
		{
			return typeof(Action<T1, T2, T3, T4>);
		}

		public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			InvokeEvent(
				EntityId, 
				EventId, 
				new object[]
				{
					arg1, 
					arg2, 
					arg3, 
					arg4
				});
		}
	}

    public class GenericEventClosure<T1, T2, T3, T4, T5>
	{
		private delegate void Action5(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

		private readonly Guid EntityId;

		private readonly int EventId;

		private readonly InvokeEventCallabck InvokeEvent;

        public GenericEventClosure(Guid entity_id, int event_id, InvokeEventCallabck invokeevent)
        {
			EntityId = entity_id;
			EventId = event_id;
			InvokeEvent = invokeevent;
		}

		public static Type GetDelegateType()
		{
			return typeof(Action5);
		}

		public void Run(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			InvokeEvent(
				EntityId, 
				EventId, 
				new object[]
				{
					arg1, 
					arg2, 
					arg3, 
					arg4, 
					arg5
				});
		}
	}
}
