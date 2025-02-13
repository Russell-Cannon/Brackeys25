using Godot;

//A collection of functions to slerp between two values.
//This is based on Freya Holmier's blog post about this
public class Slerp {
    public static Quaternion LerpSmooth(Quaternion from, Quaternion to, float deltatime, float halflife) {
        from = from.Normalized();
        to = to.Normalized();
        return from.Slerp(to, deltatime/halflife);
    }
    public static Vector3 LerpSmooth(Vector3 from, Vector3 to, float deltatime, float halflife, bool radial) {
        return new Vector3(LerpSmooth(from.X, to.X, deltatime, halflife, radial), LerpSmooth(from.Y, to.Y, deltatime, halflife, radial), LerpSmooth(from.Z, to.Z, deltatime, halflife, radial));
    }
    public static Vector2 LerpSmooth(Vector2 from, Vector2 to, float deltatime, float halflife, bool radial) {
        return new Vector2(LerpSmooth(from.X, to.X, deltatime, halflife, radial), LerpSmooth(from.Y, to.Y, deltatime, halflife, radial));
    }
    public static Vector3 LerpSmooth(Vector3 from, Vector3 to, float deltatime, float halflife) {
        return new Vector3(LerpSmooth(from.X, to.X, deltatime, halflife), LerpSmooth(from.Y, to.Y, deltatime, halflife), LerpSmooth(from.Z, to.Z, deltatime, halflife));
    }
    public static Vector2 LerpSmooth(Vector2 from, Vector2 to, float deltatime, float halflife) {
        return new Vector2(LerpSmooth(from.X, to.X, deltatime, halflife), LerpSmooth(from.Y, to.Y, deltatime, halflife));
    }
    public static float LerpSmooth(float from, float to, float deltatime, float halflife) {
        return LerpSmooth(from, to, deltatime, halflife, false);
    }
    public static float LerpSmooth(float from, float to, float deltatime, float halflife, bool radial) {
        // return Mathf.LerpAngle(a, b, dt*(1/h));//framerate dependant
        if (radial)
            to = Mathf.LerpAngle(from, to, 1);//fix over tau interp breaking
        return to+(from-to)*Mathf.Pow(2f, -deltatime/halflife);
    }
}
