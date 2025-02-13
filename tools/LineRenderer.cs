using Godot;
using System;

public partial class LineRenderer : MeshInstance3D
{
    public ImmediateMesh mesh = new();
    [Export] public Material meshMaterial;
    int VertResolution = 5;
    [Export] public float LineThickness = 1, TextureSize;
    [Export] public bool Tiled;
	[Export] bool Emitting;
	[Export] Curve WidthOverTime;
	[Export] Node3D Target;
    public Camera3D cam = null;
	double time;
    public override void _Ready() {
        Mesh = mesh;
        cam = GetViewport().GetCamera3D();
    }
    public override void _Process(double delta) {
        if (!Visible) {
            QueueFree();
            return;   
        }

		if (Emitting) {
			time += delta;
			DrawLine(delta);
		}
    }
    public virtual void DrawLine(double delta) {
		mesh.ClearSurfaces();

        mesh.SurfaceBegin(Mesh.PrimitiveType.TriangleStrip, meshMaterial);

        float distance = GlobalPosition.DistanceTo(Target.GlobalPosition);
        Vector3 Direction = GlobalPosition.DirectionTo(Target.GlobalPosition);
        int NumVerts = (int)(distance * VertResolution);
        NumVerts = NumVerts < 2 ? 2 : NumVerts;
        for (int i = 0; i < NumVerts; i++) {//partialPosition-GlobalPosition
            float index = (float)i/(NumVerts-1);
            Vector3 IndexPosition = GlobalPosition.Lerp(distance*Direction, index);
            Vector3 dir = GlobalPosition.DirectionTo(cam.GlobalPosition).Cross(Direction).Normalized();
            dir *= LineThickness/2f;
            for (int j = 0; j < 2; j++) {
                if (Tiled)
                    mesh.SurfaceSetUV(new Vector2(j/1f, distance-distance*index*TextureSize));
                else 
                    mesh.SurfaceSetUV(new Vector2(j/1f,index));
                int offset = (int)Mathf.Lerp(-1, 1, j/1f);
                mesh.SurfaceAddVertex(ToLocal(IndexPosition + dir*offset));
            }
        }
        mesh.SurfaceEnd();
    }

	public void Emit() {
		Emitting = true;
		time = 0;
	}
	public void Stop() {
		mesh.ClearSurfaces();
		Emitting = false;
	}
}
