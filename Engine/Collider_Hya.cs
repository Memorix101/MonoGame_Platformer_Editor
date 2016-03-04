using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLikePlatfromer
{
    enum ColliderType
    {
        Box,
        Circle,
        Point,
        Raycast,
    }

    enum CollsionDir
    {
        None,
        Left,
        Right,
        Up,
        Down,
    }

    struct CollisionData
    {
        public CollsionDir colldir;
        public Vector2 delta;

        public CollisionData(CollsionDir cd, Vector2 d)
        {
            colldir = cd;
            delta = d;
        }
    }

    class Collider
    {
        public ColliderType type;
        public Vector2 center;

        public Collider(Vector2 point, ColliderType t)
        {
            center = point;
            type = t;
        }
 

    }

    class BoxCollider : Collider
    {
        
        public Rectangle rect;

        public BoxCollider(Rectangle r) : base(r.Location.ToVector2(), ColliderType.Box)
        {
            rect = r;
        }

        public CollisionData intersectsBox(BoxCollider other)
        {
            CollisionData cd = new CollisionData(CollsionDir.None, Vector2.Zero);

            float deltaX = other.center.X - center.X;
            float pointX = (other.rect.Width / 2 + rect.Width / 2) - Math.Abs(deltaX);

            if(pointX <= 0)
            {
                return cd;
            }

            float deltaY = other.center.Y - center.Y;
            float pointY = (other.rect.Height / 2 + rect.Height / 2) - Math.Abs(deltaY);
            
            if (pointY <= 0)
            {
                return cd;
            }
            

            if(pointX < pointY)
            {
                float signX = Math.Sign(deltaX);
                cd.delta = new Vector2(pointX * signX, 0f);

                if (signX < 0)
                    cd.colldir = CollsionDir.Right;
                else
                    cd.colldir = CollsionDir.Left;
            }
            else
            {
                float signY = Math.Sign(deltaY);
                cd.delta = new Vector2(0f, pointY * signY);

                if (signY < 0)
                    cd.colldir = CollsionDir.Up;
                else
                    cd.colldir = CollsionDir.Down;
            }

            return cd;
        }
        
    }
}
