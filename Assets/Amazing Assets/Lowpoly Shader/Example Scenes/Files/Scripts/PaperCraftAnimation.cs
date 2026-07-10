// Lowpoly Shader <https://u3d.as/2A5c>
// Copyright (c) Amazing Assets <https://amazingassets.world>
 
using UnityEngine;


namespace AmazingAssets.LowpolyShader.Examples
{
    public class PaperCraftAnimation : MonoBehaviour
    {
        //Variables //////////////////////////////////////////////////////////////////
        Animation anim;
        public AnimationClip animClip;

        public float stepLength = 0.5f;
        float deltaTime = 0;
        float deltaStep = 0;

        //Functions ////////////////////////////////////////////////////////////
        void Start()
        {
            anim = GetComponent<Animation>();
        }


        void Update()
        {
            deltaTime += Time.deltaTime;
            deltaStep += Time.deltaTime;


            if (deltaStep > stepLength)
            {
                //Just play one frame of the animation
                anim[animClip.name].time = deltaTime;
                anim.Play();

                //One frame, Carl
                deltaStep = 0;
            }
            else
                anim.Stop();
        }
    }
}
