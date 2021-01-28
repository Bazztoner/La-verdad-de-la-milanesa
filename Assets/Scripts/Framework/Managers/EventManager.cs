using System.Collections.Generic;

public class EventManager
{
    public delegate void EventReceiver(params object[] parameterContainer);
    private static Dictionary<EventType, EventReceiver> _events;

    /// <summary>
    /// Llamamos a este método para suscribirnos a eventos
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="listener"></param>
    public static void SubscribeToEvent(EventType eventType, EventReceiver listener)
    {
        if (_events == null)
            _events = new Dictionary<EventType, EventReceiver>();

        if (!_events.ContainsKey(eventType))
            _events.Add(eventType, null);

        _events[eventType] += listener;
    }

    /// <summary>
    /// Llamamos a este método para desuscribirnos de eventos
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="listener"></param>
    public static void UnsubscribeToEvent(EventType eventType, EventReceiver listener)
    {
        if (_events != null)
        {
            if (_events.ContainsKey(eventType))
                _events[eventType] -= listener;
        }
    }

    /// <summary>
    /// Llamamos a esta función para disparar un evento
    /// </summary>
    /// <param name="eventType"></param>
    public static void TriggerEvent(EventType eventType)
    {
        TriggerEvent(eventType, null);
    }

    /// <summary>
    /// Dispara el evento
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="parametersWrapper"></param>
    public static void TriggerEvent(EventType eventType, params object[] parametersWrapper)
    {
        if (_events == null)
        {
            UnityEngine.Debug.LogWarning("No events subscribed");
            return;
        }

        if (_events.ContainsKey(eventType))
        {
            if (_events[eventType] != null)
                _events[eventType](parametersWrapper);
        }
    }
}

/// <summary>
/// Lista de eventos posibles
/// </summary>
public enum EventType
{
    CUSTOMER_HAPPY,
    CUSTOMER_ANGRY,
    CUSTOMER_TIMEOUT
}