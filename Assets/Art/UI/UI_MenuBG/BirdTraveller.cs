using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ©️YEARHERE Designed and Programmed by Joshua Thompson. All rights reserved
/// </summary>

namespace AstralCandle.UIAI{
    public class BirdTraveller : Turtle{
        [SerializeField] float breakingDistance = 100;
        Camera _c;
        Camera Camera => _c ??= Camera.main;

        Vector2 UISpaceBounds_BL => ScreenToUISpace(Camera.main.ViewportToScreenPoint(new Vector2(0,0)));
        Vector2 UISpaceBounds_TR => ScreenToUISpace(Camera.main.ViewportToScreenPoint(new Vector2(1,1)));

        Vector2? _target;
        Vector2 Target => _target ??= GetRndPos();

        Vector2 GetRndPos() => new(
            UnityEngine.Random.Range(UISpaceBounds_BL.x, UISpaceBounds_TR.x),
            UnityEngine.Random.Range(UISpaceBounds_BL.y, UISpaceBounds_TR.y)
        );

        protected override Vector2[] Move() {
            if(Vector2.Distance(Target, WorldPosition) < breakingDistance * .1f){ _target = null; }
            
            Vector2 arrive = Steering.Arrive(Target, breakingDistance);
            return new Vector2[] { arrive };
        }
    }
}