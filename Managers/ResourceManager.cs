﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace ReMod.Core.Managers
{
    public static class ResourceManager
    {
        private static readonly Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        private static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

        public static Texture2D LoadTexture(string resourceName, byte[] bytes)
        {
            if (Textures.ContainsKey(resourceName))
            {
                throw new ArgumentException("Resource already exists", nameof(resourceName));
            }
            
            var texture = new Texture2D(1, 1);
            ImageConversion.LoadImage(texture, bytes);
            texture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            texture.wrapMode = TextureWrapMode.Clamp;

            Textures.Add(resourceName, texture);

            return texture;
        }

        public static Texture2D GetTexture(string resourceName)
        {
            return Textures.ContainsKey(resourceName) ? Textures[resourceName] : null;
        }

        public static Sprite LoadSprite(string resourceName, byte[] bytes)
        {
            var texture = GetTexture(resourceName);
            if (texture == null)
            {
                texture = LoadTexture(resourceName, bytes);
            }

            var rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            var pivot = new Vector2(0.5f, 0.5f);
            var border = Vector4.zero;
            var sprite = Sprite.CreateSprite_Injected(texture, ref rect, ref pivot, 100.0f, 0, SpriteMeshType.Tight, ref border, false);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;

            Sprites.Add(resourceName, sprite);

            return sprite;
        }

        public static Sprite GetSprite(string resourceName)
        {
            return Sprites.ContainsKey(resourceName) ? Sprites[resourceName] : null;
        }
    }
}
