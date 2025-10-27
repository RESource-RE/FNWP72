// Decompiled with JetBrains decompiler
// Type: Mortar.SoundManager
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using FruitNinja;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Mortar
{

    public class SoundManager
    {
      public float soundFadeOut;
      private Dictionary<string, Microsoft.Xna.Framework.Audio.SoundEffect> sfxs = new Dictionary<string, Microsoft.Xna.Framework.Audio.SoundEffect>();
      private LinkedList<SoundEffectInstance> sysManagedSfx = new LinkedList<SoundEffectInstance>();
      private static float sfx_volume = 1f;
      private static float mus_volume = 1f;
      private static SoundManager instance = new SoundManager();
      private bool customMusic;
      private bool allowMusic;
      private Song currentSong;
      private static bool curr = false;
      private static bool prev = false;

      public static int SOUND_MANAGER_HEAP_SIZE => 524288 /*0x080000*/;

      public static byte DEFAULT_NOTE => 64 /*0x40*/;

      public static SoundManager GetInstance() => SoundManager.instance;

      public void Initialise(string project)
      {
        this.Initialise(project, SoundManager.SOUND_MANAGER_HEAP_SIZE);
      }

      public void Initialise(string project, int heap_size) => this.CacheSFX();

      public void Destroy()
      {
      }

      private void CacheSFX()
      {
        try
        {
          string str = TheGame.instance.Content.Load<string>("sfxCacheFile.txt");
          char[] separator = new char[1]{ ',' };
          foreach (string key in str.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            Microsoft.Xna.Framework.Audio.SoundEffect soundEffect = (Microsoft.Xna.Framework.Audio.SoundEffect) null;
            try
            {
              soundEffect = TheGame.instance.Content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>($"sound/{key}.wav");
            }
            catch (Exception ex)
            {
            }
            this.sfxs.Add(key, soundEffect);
          }
        }
        catch (Exception ex)
        {
        }
      }

      public MortarSound SFXPlay(string filename)
      {
        this.SFXPlay(filename, 0U);
        return new MortarSound();
      }

      public void SFXPlay(string filename, uint flags)
      {
        this.SFXPlay(filename, flags, (MortarSound) null);
      }

      public void SFXPlay(string filename, uint flags, MortarSound snd)
      {
        this.SFXPlay(filename, flags, snd, SoundManager.DEFAULT_NOTE);
      }

      public void SFXPlay(string filename, uint flags, MortarSound snd, byte note)
      {
        this.SFXPlay(filename, flags, snd, note, -1);
      }

      public void SFXPlay(string filename, uint flags, MortarSound snd, byte note, int pitch)
      {
        this.SFXPlay(filename, flags, snd, note, pitch, false);
      }

      public void SFXPlay(
        string filename,
        uint flags,
        MortarSound snd,
        byte note,
        int pitch,
        bool loop)
      {
        Microsoft.Xna.Framework.Audio.SoundEffect soundEffect;
        if (!this.sfxs.TryGetValue(filename, out soundEffect))
        {
          soundEffect = (Microsoft.Xna.Framework.Audio.SoundEffect) null;
          try
          {
            string assetName = $"sound/{filename}.wav";
            soundEffect = TheGame.instance.Content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>(assetName);
          }
          catch (Exception ex)
          {
          }
          this.sfxs.Add(filename, soundEffect);
        }
        if (soundEffect == null)
          return;
        SoundEffectInstance instance = soundEffect.CreateInstance();
        if (snd != null)
          snd.inst = instance;
        else
          this.sysManagedSfx.AddLast(instance);
        if (!Game.game_work.soundEnabled)
          return;
        instance.IsLooped = loop;
        instance.Volume = SoundManager.sfx_volume;
        instance.Play();
      }

      public void SFXStop(int index)
      {
      }

      public void SFXStop(string filename)
      {
      }

      public void SFXPause(int index)
      {
      }

      public void SFXPause(string filename)
      {
      }

      public void SFXPauseAll()
      {
      }

      public static MortarSound CreateNewSound() => new MortarSound();

      public void AllowMusic()
      {
        this.allowMusic = true;
        MediaPlayer.Stop();
      }

      public bool CustomMusic
      {
        get => this.customMusic;
        set => this.customMusic = value;
      }

      private void PlaySong(string name)
      {
        this.currentSong = TheGame.instance.Content.Load<Song>(name);
        SoundManager.mus_volume = 0.01f;
        this.ApplyMusicVolume();
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(this.currentSong);
      }

      public void SongPlay(string filename)
      {
        if (!this.allowMusic)
          return;
        try
        {
          this.PlaySong($"music/{filename}.wma");
        }
        catch (Exception ex1)
        {
          this.currentSong = (Song) null;
          try
          {
            this.PlaySong("music/" + filename);
          }
          catch (Exception ex2)
          {
            this.currentSong = (Song) null;
          }
        }
      }

      public bool UserPlayingMusic() => MediaPlayer.State == MediaState.Playing;

      public void SongSwitchPlay(string filename)
      {
      }

      public void SongStop()
      {
      }

      public void SongPause()
      {
      }

      public void SongResume()
      {
      }

      public void SongSetStreaming(bool streaming)
      {
      }

      public void SongSetMemorySize(int size)
      {
      }

      public void MuteSound() => this.MuteSound(true);

      public void MuteSound(bool mute)
      {
      }

      public void Update(float dt)
      {
        LinkedListNode<SoundEffectInstance> next;
        for (LinkedListNode<SoundEffectInstance> node = this.sysManagedSfx.First; node != null; node = next)
        {
          next = node.Next;
          if (node.Value.State == SoundState.Stopped)
            this.sysManagedSfx.Remove(node);
        }
        if (this.allowMusic)
        {
          SoundManager.curr = !Game.game_work.musicEnabled;
          if (SoundManager.prev != SoundManager.curr)
          {
            SoundManager.prev = SoundManager.curr;
            try
            {
              MediaPlayer.IsMuted = SoundManager.curr;
            }
            catch
            {
            }
          }
        }
        if (this.currentSong != (Song) null && this.allowMusic && (double) SoundManager.mus_volume < 1.0)
        {
          SoundManager.mus_volume += 0.0166666675f;
          if ((double) SoundManager.mus_volume > 1.0)
            SoundManager.mus_volume = 1f;
        }
        this.ApplyMusicVolume();
      }

      private void ApplyMusicVolume()
      {
        MediaPlayer.Volume = SoundManager.mus_volume * 0.5f;
        MediaPlayer.IsMuted = !this.allowMusic || !Game.game_work.musicEnabled || (double) SoundManager.mus_volume <= 0.05000000074505806;
      }

      public void Draw(SpriteBatch batch)
      {
      }

      public void Release(MortarSound obj)
      {
      }

      public void ReleaseAll()
      {
      }

      public void SetSFXVolume(float amount)
      {
      }
    }
}
