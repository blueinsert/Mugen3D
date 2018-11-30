using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class SoundItem : IComparable<SoundItem>
    {
        public float delay { get; set; }
        public AudioClip clip { get; private set; }
        public float volume { get; private set; }

        public SoundItem(AudioClip clip, float delay, float volume)
        {
            this.clip = clip;
            this.delay = delay;
            this.volume = volume;
        }

        public int CompareTo(SoundItem other)
        {
            return delay.CompareTo(other.delay);
        }
    }

    public class SoundChannel : MonoBehaviour
    {
        public enum SoundChannelType
        {
            Default = 0,
            Reserve1,
            Reserve2,
            Reserve3,
        }

        private AudioSource m_audioSource;

        private void Awake()
        {
            m_audioSource = gameObject.AddComponent<AudioSource>();
        }

        private PriorityQueue<SoundItem> m_playQueue = new PriorityQueue<SoundItem>();

        private void PlaySound(AudioClip clip, float volume)
        {
            m_audioSource.PlayOneShot(clip, volume);
        }

        public void Play(AudioClip clip, float volume, float delay)
        {
            m_playQueue.Enqueue(new SoundItem(clip, delay, volume));
        }

        public void Update()
        {
            if (m_playQueue.Count() == 0)
                return;
            var e = m_playQueue.GetEnumerator();
            while (e.MoveNext())
            {
                e.Current.delay -= Time.deltaTime;
            }
            while (m_playQueue.Count() > 0 && m_playQueue.Peek().delay <= 0)
            {
                var item = m_playQueue.Dequeue();
                PlaySound(item.clip, item.volume);
            }
        }
    }

    public class SoundPlayer : Singleton<SoundPlayer>
    {
        private Dictionary<string, AudioClip> m_cache = new Dictionary<string, AudioClip>();
        private Dictionary<SoundChannel.SoundChannelType, SoundChannel> m_soundChannelDic = new Dictionary<SoundChannel.SoundChannelType, SoundChannel>();

        private void Awake()
        {
            for(int i = 0; i < System.Enum.GetNames(typeof(SoundChannel.SoundChannelType)).Length; i++)
            {
                SoundChannel channel = gameObject.AddComponent<SoundChannel>();
                m_soundChannelDic.Add((SoundChannel.SoundChannelType)i, channel);
            }    
        }

        public void Play(string soundName, float delay = 0, float volume = 1, SoundChannel.SoundChannelType channel = SoundChannel.SoundChannelType.Default)
        {
            AudioClip clip = null;
            if (m_cache.ContainsKey(soundName))
            {
                clip = m_cache[soundName];
            }
            else
            {
                clip = ResourceLoader.Load<AudioClip>("Sound/" + soundName);
                if (clip != null)
                {
                    m_cache.Add(soundName, clip);
                }
                else
                {
                    Debug.LogError("can't load audioClip: " + soundName);
                }
            }
            if (clip == null)
                return;
            m_soundChannelDic[channel].Play(clip, volume, delay);
        }
    }
}
