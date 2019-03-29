using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Little helper class that takes in a body, shadow, leap height, leap speed, animationCurve, and end positions and translates over the two points with a nice little animation and shadow morph.
// Should work great for enemies, items, projectiles, anything with an arcing trajectory.
// Caveats: shadow most be a child gameObject of the body!
public class TimedLeapifier
{
    GameObject _body;
    GameObject _shadow;
    float _leapHeight;
    float _leapTime;
    Vector3 _leapStart;
    Vector3 _leapEnd;
    AnimationCurve _curve;

    Vector3 _shadowLocalPos;
    Vector3 _shadowLocalScale;

    float _timeStart;
    float _timeEnd;

    public TimedLeapifier(GameObject body, GameObject shadow, float leapHeight, float leapTime, Vector3 leapEnd, AnimationCurve curve)
    {
        _body = body;
        _shadow = shadow;
        _leapHeight = leapHeight;
        _leapTime = leapTime;
        _leapEnd = leapEnd;
        _curve = curve;

        _leapStart = _body.transform.position;
        _shadowLocalPos = _shadow.transform.localPosition;
        _shadowLocalScale = _shadow.transform.localScale;

        _timeStart = Time.time;
        _timeEnd = Time.time + leapTime;
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
    public virtual bool OnUpdate()
    {
        if (Time.time < _timeEnd) {
            // Lerp toward the destination using a percentage of the total time passed.
            float t = (Time.time - _timeStart) / (_timeEnd - _timeStart);
            Vector3 pos = Vector2.Lerp(_leapStart, _leapEnd, t);
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
