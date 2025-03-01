using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Effects
{
    public class WaterHoseParticles : MonoBehaviour
    {
        public static float lastSoundTime;
        public float force = 1;


        //private ParticleCollisionEvent[] m_CollisionEvents = new ParticleCollisionEvent[16];
        private ParticleSystem m_ParticleSystem;
        private List<ParticleCollisionEvent> m_CollisionEvents = new List<ParticleCollisionEvent>();
        private void Start()
        {
            m_ParticleSystem = GetComponent<ParticleSystem>();
        }


        private void OnParticleCollision(GameObject other)
        {
            int safeLength = m_ParticleSystem.GetSafeCollisionEventSize();

            if (m_CollisionEvents.Count < safeLength)
            {
                ParticleCollisionEvent[] p_CollisionEvents = m_CollisionEvents.ToArray();
                p_CollisionEvents = new ParticleCollisionEvent[safeLength];
            }

            int numCollisionEvents = m_ParticleSystem.GetCollisionEvents(other, m_CollisionEvents);
            int i = 0;

            while (i < numCollisionEvents)
            {
                if (Time.time > lastSoundTime + 0.2f)
                {
                    lastSoundTime = Time.time;
                }

                var col = m_CollisionEvents[i].colliderComponent.GetComponent<Collider>();

                if (col.attachedRigidbody != null)
                {
                    Vector3 vel = m_CollisionEvents[i].velocity;
                    col.attachedRigidbody.AddForce(vel*force, ForceMode.Impulse);
                }

                other.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);

                i++;
            }
        }
    }
}
