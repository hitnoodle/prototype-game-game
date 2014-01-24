using UnityEngine;

public class Main : MonoBehaviour {
	// Use this for initialization
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
	void Start () {
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Local
		//Create params
		var Params = new FutileParams(true, true, false, false) { origin = new Vector2(0.0f, 0.0f) };
		//Params.AddResolutionLevel(480.0f, 1.0f, 1.0f, "_x1"); //iPhone
		//Params.AddResolutionLevel(960.0f, 2.0f, 2.0f, "_x2"); //iPhone Retina
		Params.AddResolutionLevel(768.0f, 1.0f, 1.0f, ""); //i
		
		//Initialize
		Futile.instance.Init(Params);
		Futile.atlasManager.LoadAtlas("Atlases/atlas");
		Futile.atlasManager.LoadAtlas("Atlases/atlas-pixel");
		Futile.atlasManager.LoadFont("font", "font", "Atlases/font", 0, 0);
		//Futile.atlasManager.LoadFont("visitor", "visitor" + Futile.resourceSuffix, "Atlases/visitor" + Futile.resourceSuffix, 0, 0);
		//Futile.atlasManager.LoadFont("visitor-big", "visitor-big" + Futile.resourceSuffix, "Atlases/visitor-big" + Futile.resourceSuffix, 0, 0);
		//Futile.atlasManager.LoadFont("visitor-small", "visitor-small" + Futile.resourceSuffix, "Atlases/visitor-small" + Futile.resourceSuffix, 0, 0);
		
		//Create
		StateManager.instance.setup(new StateFactory());
		Futile.stage.AddChild(StateManager.instance);
	}
}
