using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPart : MonoBehaviour {

	StateMachine<MainPart> sm_;
	List<GameObject> objectList_;

	void Awake() {
		sm_ = new StateMachine<MainPart>(stateInit_g_);
		objectList_ = new List<GameObject>();
	}

	void OnDestroy() {
		sm_ = null;
		objectList_ = null;
	}
	void FixedUpdate() {
	}

	void Update() {
		sm_.Update(this);

		for (var i = objectList_.Count - 1; 0 <= i; i--) {
			var go = objectList_[i];
			if (!go) {
				objectList_.RemoveAt(i);
				continue;
			}
			var block = go.GetComponent<Block>();
			if (!block) continue;
			var v = block.firstPosition - go.transform.position;
			if (v.sqrMagnitude < 1f) continue;
			GameObject.Destroy(go, 10f);
			objectList_.RemoveAt(i);
		}
	}

	int index_;
	Vector3 createBasePos() {
		var pos = new Vector3(
			(index_ % 2) * 8,
			0.5f,
			(index_ / 2) * 8
		);
		index_ = (index_ + 1) % 4;
		return pos;
	}


	static Collider[] results_ = new Collider[256];
	static StateMachine<MainPart>.StateFunc stateInit_g_ = (_evt) => {
		if (_evt.sm.time < 1f) return null;
		var blockPrefab = Resources.Load<GameObject>("block");
		var size = new Vector3(6, 16, 6);
		for (var i = 0; i < 8; i++) {
			var basePos = _evt.owner.createBasePos();
			var self = _evt.owner;
			{
				var halfSize = size * 0.5f;
				var pos2 = basePos + new Vector3((size.x - 1f), (size.y - 1f), (size.z - 1f)) * 0.5f;
				int rCount = Physics.OverlapBoxNonAlloc(pos2, halfSize * 1.1f, results_);
				for (var j = 0; j < rCount; j++) {
					var col = results_[j];
					var block = col.GetComponent<Block>();
					if (!block) continue;
					Debug.Log("destroy: " + block.GetInstanceID());
					col.enabled = false;
					GameObject.Destroy(block);
				}
			}

			self.createTowner(basePos, size);
		}
		return stateMain_g_;
	};

	static StateMachine<MainPart>.StateFunc stateMain_g_ = (_evt) => {
		if (_evt.sm.time < 2f) return null;
		var blockPrefab = Resources.Load<GameObject>("block");
		var size = new Vector3(6, 16, 6);
			var basePos = _evt.owner.createBasePos();
		var self = _evt.owner;
		{
			var halfSize = size * 0.5f;
			var pos2 = basePos + new Vector3((size.x - 1f), (size.y - 1f), (size.z - 1f)) * 0.5f;
			int rCount = Physics.OverlapBoxNonAlloc(pos2, halfSize * 1.1f, results_);
			for (var j = 0; j < rCount; j++) {
				var col = results_[j];
				var block = col.GetComponent<Block>();
				if (!block) continue;
				Debug.Log("destroy: " + block.GetInstanceID());
				col.enabled = false;
				GameObject.Destroy(block);
			}
		}
		self.createTowner(basePos, size);
		return stateMain_g_;
	};

	void createTowner(Vector3 basePos, Vector3 size) {
		var blockPrefab = Resources.Load<GameObject>("block");
		var self = this;
		var world = self.transform;
		for (var xi = 0; xi < size.x; xi++) {
			for (var zi = 0; zi < size.z; zi++) {
				if ((MathUtil.isIn(xi, 1, size.x - 2) && MathUtil.isIn(zi, 1, size.z - 2))) continue;
				for (var yi = 0; yi < size.y; yi++) {
					var pos = basePos + new Vector3(xi, yi, zi);
					var block = GameObject.Instantiate(blockPrefab, pos, Quaternion.identity, world);
					var bl = block.AddComponent<Block>();
					bl.firstPosition = pos;
					self.objectList_.Add(block);
				}
			}
		}
	}

	class Block : MonoBehaviour {
		public Vector3 firstPosition;
	}

	static class MathUtil {
		public static bool isIn(int a, int min, int max) {
			return min <= a && a < max;
		}
		public static bool isIn(float a, float min, float max) {
			return min <= a && a <= max;
		}
	}

}
