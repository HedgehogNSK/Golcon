using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hedge.UnityUI
{
    namespace Hedge.UnityUI
    {
        [RequireComponent(typeof(SpriteRenderer))]
        public class SpriteHideEffect : HideEffect
        {
            public bool fade = true;
            [Range(0.0f,0.9f)]
            public float lifePartBeforeFade = 0.3f;
            protected SpriteRenderer sprite;

            float time_of_creation;
            // Use this for initialization
            protected override void Awake()
            {
                time_of_creation = Time.time;
                base.Awake();
                sprite = GetComponent<SpriteRenderer>();
                if (fade) PlayEffect += FadeImage;

            }
            
            private void FadeImage()
            {

                if (Time.time - time_of_creation > lifePartBeforeFade)
                {
                    float current_life_part = (Time.time - time_of_creation) / life_time - lifePartBeforeFade;
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a * (1 - current_life_part)); 
                }
              
            }

        }

    }
}

