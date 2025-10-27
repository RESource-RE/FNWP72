// Decompiled with JetBrains decompiler
// Type: FruitNinja.ScrollingMenu
// Assembly: FNWP72, Version=1.7.2.1, Culture=neutral, PublicKeyToken=null
// MVID: D58381B4-946C-48A2-ACC2-E62A5FC74F74
// Assembly location: C:\Users\Texture2D\Documents\WP\FNWP72.dll

using Microsoft.Xna.Framework;
using Mortar;
using System.Collections.Generic;

namespace FruitNinja
{

    public class ScrollingMenu : HUDControl
    {
      public const float LIST_FRICTION = 0.9f;
      protected int m_touchInRegion;
      protected Vector3 m_touchStartPos;
      protected Vector3 m_touchStartOffset;
      protected Vector3 m_vel;
      protected float m_itemHeight;
      protected float m_height;
      protected float m_width;
      protected float m_totalHeight;
      protected float m_totalWidth;
      protected List<ScrollingMenuItem> m_list = new List<ScrollingMenuItem>();
      protected int m_itemClosestToZero;
      protected int m_overideLockTo;
      protected float m_velocityFix;
      protected bool m_hasMovedList;
      protected bool m_isLockedOn;
      protected bool m_cullItemsOutOfBounds;
      public bool m_disabled;
      public Vector3 m_offset;
      public MortarRectangleDec m_initialTouchRegion;
      public MortarRectangleDec m_scrollTouchRegion;

      protected void DestroyList()
      {
        SlashEntity.ModPowerMask &= 4294967231U;
        this.m_list.Clear();
      }

      public ScrollingMenu()
      {
        this.m_totalWidth = 0.0f;
        this.m_totalHeight = 0.0f;
        this.m_itemClosestToZero = 0;
        this.m_width = Game.SCREEN_WIDTH / 2f;
        this.m_height = Game.SCREEN_HEIGHT;
        this.m_itemHeight = Game.SCREEN_HEIGHT;
        this.m_velocityFix = 1f;
        this.m_hasMovedList = false;
        this.m_overideLockTo = -1;
        this.m_isLockedOn = false;
        this.m_touchInRegion = -1;
        this.m_disabled = false;
        this.m_initialTouchRegion = new MortarRectangleDec((float) (-(double) this.m_width / 2.0), Game.SCREEN_HEIGHT, this.m_width / 2f, -Game.SCREEN_HEIGHT);
        this.m_scrollTouchRegion = new MortarRectangleDec((float) (-(double) this.m_width / 2.0), Game.SCREEN_HEIGHT, this.m_width / 2f, -Game.SCREEN_HEIGHT);
        this.m_deleteCall = (HUDControl.HUDControlDeletedCallback) (l => l.Release());
      }

      ~ScrollingMenu() => this.Release();

      public override void Reset()
      {
        this.m_selfCleanUp = true;
        this.m_touchInRegion = -1;
        this.m_offset = Vector3.Zero;
        this.m_touchStartPos = Vector3.Zero;
        this.m_touchStartOffset = Vector3.Zero;
        this.m_vel = Vector3.Zero;
        this.m_velocityFix = 1f;
        this.m_overideLockTo = -1;
        this.m_cullItemsOutOfBounds = false;
      }

      public override void Release() => this.DestroyList();

      public override void Init() => this.Reset();

      public override void Update(float dt)
      {
        this.m_isLockedOn = false;
        if (this.m_touchInRegion == -1 && !this.m_disabled)
        {
          this.m_touchInRegion = Game.TouchInRegion(this.m_pos.X + this.m_initialTouchRegion.left, this.m_pos.X + this.m_initialTouchRegion.right, this.m_pos.X + this.m_initialTouchRegion.bottom, this.m_pos.X + this.m_initialTouchRegion.top);
          if (Game.IsTouchDown(this.m_touchInRegion) != 2)
          {
            this.m_touchInRegion = -1;
          }
          else
          {
            SlashEntity.ModPowerMask |= 64U /*0x40*/;
            this.m_overideLockTo = -1;
            this.m_touchStartPos = Game.game_work.touchPositions[this.m_touchInRegion];
            this.m_touchStartOffset = this.m_offset;
          }
        }
        int overideLockTo = this.m_overideLockTo;
        if (this.m_touchInRegion != -1 && Game.TouchInRegion(this.m_pos.X + this.m_scrollTouchRegion.left, this.m_pos.X + this.m_scrollTouchRegion.right, this.m_pos.X + this.m_scrollTouchRegion.bottom, this.m_pos.X + this.m_scrollTouchRegion.top, this.m_touchInRegion) != this.m_touchInRegion)
        {
          if (!this.m_hasMovedList)
          {
            float num1 = this.m_pos.Y - this.m_offset.Y;
            int num2 = 0;
            this.m_overideLockTo = -1;
            float num3 = 1000000f;
            foreach (ScrollingMenuItem scrollingMenuItem in this.m_list)
            {
              if ((double) Math.ABS(num1 - this.m_pos.Y - this.m_touchStartPos.Y) < (double) num3)
              {
                num3 = Math.ABS(num1 - this.m_pos.Y - this.m_touchStartPos.Y);
                this.m_overideLockTo = num2;
              }
              num1 -= scrollingMenuItem.GetHeight();
              ++num2;
            }
            this.m_vel = Vector3.Zero;
          }
          else
          {
            float t = 0.0f;
            float y = this.m_vel.Y;
            while ((double) Math.ABS(y) > 0.05000000074505806)
            {
              y *= 0.9f;
              t += y;
            }
            if ((double) Math.ABS(t) > 0.0099999997764825821 && (double) this.m_offset.Y + (double) t <= 0.0099999997764825821 && (double) this.m_offset.Y + (double) t >= -(double) this.m_totalHeight + (double) this.m_itemHeight + 0.0099999997764825821)
            {
              float num4 = 10000f;
              float num5 = 0.0f;
              float num6 = this.m_pos.Y - this.m_offset.Y - t;
              float num7 = 0.0f;
              foreach (ScrollingMenuItem scrollingMenuItem in this.m_list)
              {
                if ((double) Math.ABS(num6 - this.m_pos.Y) < (double) num4)
                {
                  num4 = Math.ABS(num6 - this.m_pos.Y);
                  num5 = num7;
                }
                num7 -= scrollingMenuItem.GetHeight();
                num6 -= scrollingMenuItem.GetHeight();
              }
              this.m_velocityFix = Math.ABS((num5 - this.m_offset.Y) / t);
            }
          }
          this.m_hasMovedList = false;
          this.m_touchInRegion = -1;
          SlashEntity.ModPowerMask &= 4294967231U;
        }
        if (this.m_touchInRegion != -1)
        {
          this.m_velocityFix = 1f;
          this.m_vel.Y = (float) (-((double) this.m_offset.Y - ((double) this.m_touchStartOffset.Y - ((double) Game.game_work.touchPositions[this.m_touchInRegion].Y - (double) this.m_touchStartPos.Y))) * 0.5);
          if ((double) Math.ABS(Game.game_work.touchPositions[this.m_touchInRegion].Y - this.m_touchStartPos.Y) > 1.0 / 1000.0)
            this.m_hasMovedList = true;
        }
        this.m_vel *= 0.9f;
        this.m_offset += this.m_vel * this.m_velocityFix;
        Vector3 toPos = this.m_pos - this.m_offset;
        int num8 = 0;
        float num9 = 10000f;
        float num10 = 0.0f;
        this.m_itemClosestToZero = 0;
        ScrollingMenuItem scrollingMenuItem1 = (ScrollingMenuItem) null;
        float num11 = Game.SCREEN_HEIGHT / 2f;
        float num12 = (float) (-(double) Game.SCREEN_HEIGHT / 2.0);
        if (this.m_cullItemsOutOfBounds)
        {
          num11 = this.m_pos.Y;
          num12 = this.m_pos.Y - this.m_height;
        }
        foreach (ScrollingMenuItem scrollingMenuItem2 in this.m_list)
        {
          float num13 = scrollingMenuItem2.GetHeight() / 2f;
          if (this.m_overideLockTo >= 0)
          {
            if (this.m_overideLockTo == num8)
            {
              num9 = Math.ABS(toPos.Y - this.m_pos.Y);
              num10 = toPos.Y - (this.m_pos.Y - this.m_offset.Y);
              this.m_itemClosestToZero = num8;
              scrollingMenuItem1 = scrollingMenuItem2;
            }
          }
          else if ((double) Math.ABS(toPos.Y - this.m_pos.Y) < (double) num9)
          {
            num9 = Math.ABS(toPos.Y - this.m_pos.Y);
            num10 = toPos.Y - (this.m_pos.Y - this.m_offset.Y);
            this.m_itemClosestToZero = num8;
          }
          toPos.Y -= num13;
          if ((double) toPos.Y - (double) num13 > (double) num11 || (double) toPos.Y + (double) num13 < (double) num12)
            scrollingMenuItem2.SetOnscreen(false);
          else
            scrollingMenuItem2.SetOnscreen(true);
          scrollingMenuItem2.Move(toPos);
          toPos.Y -= num13;
          ++num8;
        }
        float t1 = num10 - this.m_offset.Y;
        if (!this.m_hasMovedList && (double) Math.ABS(t1) < 2.0 && (double) Math.ABS(this.m_vel.Y) < 0.5)
        {
          this.m_isLockedOn = true;
          if (overideLockTo == -1 && scrollingMenuItem1 != null)
            scrollingMenuItem1.CallClickedMenuItemCallback();
        }
        if ((double) this.m_offset.Y > 0.0 && this.m_overideLockTo < 0)
        {
          this.m_offset.Y *= 0.75f;
          this.m_vel *= 0.9f;
        }
        else if ((double) this.m_offset.Y < -(double) this.m_totalHeight + (double) this.m_height && this.m_overideLockTo < 0)
        {
          this.m_offset.Y += (float) ((-(double) this.m_totalHeight + (double) this.m_height - (double) this.m_offset.Y) * 0.25);
          this.m_vel *= 0.9f;
        }
        else
        {
          if (this.m_touchInRegion != -1 || (double) Math.ABS(this.m_vel.Y) > 0.10000000149011612)
            return;
          this.m_offset.Y += t1 * 0.1f;
        }
      }

      public override void PreDraw(float[] tintChannels)
      {
      }

      public override void Draw(float[] tintChannels)
      {
        foreach (ScrollingMenuItem scrollingMenuItem in this.m_list)
          scrollingMenuItem.Draw();
      }

      public override HUD_TYPE GetType() => HUD_TYPE.HUD_TYPE_SCROLLING_LIST;

      public override void Skip()
      {
      }

      public void ClearTouch()
      {
        this.m_touchInRegion = -1;
        this.m_hasMovedList = false;
      }

      public void AddItem(ScrollingMenuItem item)
      {
        this.m_totalWidth += item.GetWidth();
        this.m_totalHeight += item.GetHeight();
        item.SetParent(this);
        this.m_list.Add(item);
      }

      public virtual ScrollingMenuItem GetItemClosestToZero() => this.m_list[this.m_itemClosestToZero];

      public virtual int GetItemClosestToZeroIdx() => this.m_itemClosestToZero;

      public virtual float GetHeight() => this.m_height;

      public virtual float GetWidth() => this.m_width;

      public virtual void SetHeight(float v) => this.m_height = v;

      public virtual void SetWidth(float v)
      {
        this.m_width = v;
        this.m_scrollTouchRegion.left = 1.25f * (this.m_initialTouchRegion.left = (float) (-(double) v / 2.0));
        this.m_scrollTouchRegion.right = 1.25f * (this.m_initialTouchRegion.right = v / 2f);
      }

      public virtual void SetItemHeight(float v) => this.m_itemHeight = v;

      public virtual float GetItemHeight() => this.m_itemHeight;

      public int GetNumItems() => this.m_list.Count;

      public bool IsLockedIn() => this.m_isLockedOn;

      public void SetCullItems(bool val) => this.m_cullItemsOutOfBounds = val;

      public bool GetCullItems() => this.m_cullItemsOutOfBounds;
    }
}
