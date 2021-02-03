using NetBlade.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetBlade.Core.Domain
{
    public static class EntityExtension
    {
        public static List<Event> GetEvents(this Entity entity)
        {
            return entity._events = entity._events ?? new List<Event>();
        }

        public static void RegisterEvent<T>(this Entity entity, params object[] args)
            where T : DomainEvent
        {
            List<object> list = new List<object> { entity };
            object[] arguments = list.Concat(args).ToArray();
            entity.RegisterEvent((T)Activator.CreateInstance(typeof(T), arguments));
        }

        public static void RegisterEvent(this Entity entit, Event @event)
        {
            entit.GetEvents().Add(@event);
        }

        public static void RegisterInsertEvent(this Entity entit)
        {
            Type type = typeof(DomainEvent<>).MakeGenericType(entit.GetType());
            Event @event = (Event)type.GetMethod(nameof(DomainEvent<Entity>.Insert), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Invoke(null, new object[] { entit });

            entit.GetEvents().Add(@event);
        }

        public static void RegisterUpdateEvent(this Entity entit)
        {
            Type type = typeof(DomainEvent<>).MakeGenericType(entit.GetType());
            Event @event = (Event)type.GetMethod(nameof(DomainEvent<Entity>.Update), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Invoke(null, new object[] { entit });

            entit.GetEvents().Add(@event);
        }

        public static void RegisterDeleteEvent(this Entity entit)
        {
            Type type = typeof(DomainEvent<>).MakeGenericType(entit.GetType());
            Event @event = (Event)type.GetMethod(nameof(DomainEvent<Entity>.Delete), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Invoke(null, new object[] { entit });

            entit.GetEvents().Add(@event);
        }
    }
}
