using UnityEngine;

namespace Gameplay
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(EntityIdentity))]
    public sealed class VisualFeedbackEventComponent : MonoBehaviour
    {
        private VisualFeedbackEventType eventType;
        private EntityIdentity source;
        private int eventVersion;

        public VisualFeedbackEventType EventType => eventType;
        public EntityIdentity Source => source;
        public int EventVersion => eventVersion;

        private void Awake()
        {
            ResetRuntimeState();
        }

        public void Raise(VisualFeedbackEventType eventType, EntityIdentity source)
        {
            this.eventType = eventType;
            this.source = source;
            eventVersion++;
        }

        public void ResetRuntimeState()
        {
            eventType = VisualFeedbackEventType.None;
            source = null;
            eventVersion = 0;
        }
    }
}
