using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Little helper class that takes in a body, shadow, leap height, leap speed, animationCurve, and end positions and translates over the two points with a nice little animation and shadow morph.
// Should work great for enemies, items, projectiles, anything with an arcing trajectory.
// Caveats: shadow most be a child gameObject of the body!
public class Leapifier {
    GameObject _body;
    GameObject _shadow;
    float _leapHeight;
    float _leapSpeed;
    Vector3 _leapStart;
    Vector3 _leapEnd;
    AnimationCurve _curve;

    Vector3 _lastGroundPos;
    Vector3 _shadowLocalPos;
    Vector3 _shadowLocalScale;

    public Leapifier(GameObject body, GameObject shadow, float leapHeight, float leapSpeed, Vector3 leapEnd, AnimationCurve curve)
    {
        _body = body;
        _shadow = shadow;
        _leapHeight = leapHeight;
        _leapSpeed = leapSpeed;
        _leapEnd = leapEnd;
        _curve = curve;

        _leapStart = _body.transform.position;
        _lastGroundPos = _leapStart;
        _shadowLocalPos = _shadow.transform.localPosition;
        _shadowLocalScale = _shadow.transform.localScale;
    }

    // Very important call to Reset the shadow to it's default local position and local scale if the leap is interrupted.
    // Call this so the shadow local position and local scale don't get completely messed up.
    public void Reset()
    {
        _shadow.transform.localPosition = _shadowLocalPos;
        _shadow.transform.localScale = _shadowLocalScale;
    }

    // Increments the leap from start to end by Time.deltaTime.
    // Returns true if the leaping object has reached it's end destination.
    public bool OnUpdate()
    {
        if (Vector2.Distance(_lastGroundPos, _leapEnd) > 0.1f) {
            // Move toward the leap destination using the true ground position
            Vector3 pos = Vector2.MoveTowards(_lastGroundPos, _leapEnd, _leapSpeed * Time.deltaTime);
            _lastGroundPos = pos;

            // offset the gameObject position with a jumping arc after the fact
            float t = (_leapStart.x - _body.transform.position.x) / (_leapStart.x - _leapEnd.x); // get the percentage of how close we are to the point
            var tCurveEval = _curve.Evaluate(t);
            Vector3 leapVec = Vector3.up * tCurveEval * _leapHeight;
            _body.transform.position = pos + leapVec; // add on the current leap vector
            _shadow.transform.localPosition = _shadowLocalPos - leapVec; // subtract leap vector to move the shadow down (TODO: this is hacky but it would take a whole lot of changes to make the shadow the parent object instead)
            _shadow.transform.localScale = Vector3.Lerp(_shadowLocalScale, _shadowLocalScale / _leapHeight, tCurveEval); // scale the shadow based on the leapCurve. The higher the enemy, the smaller the shadow.

            return false;
        }

        Reset();
        return true;
    }
}
