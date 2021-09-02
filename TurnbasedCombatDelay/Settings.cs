using UnityModManagerNet;

namespace TurnbasedCombatDelay {
    public class Settings : UnityModManager.ModSettings {
        public float Delay = 4;

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }
    }
}
