from flask import Flask, request, jsonify
import joblib
from pathlib import Path
import numpy as np

MODEL_PATH = Path(__file__).parent / "model.pkl"

app = Flask(__name__)
model = joblib.load(MODEL_PATH)

FEATURE_ORDER = [
    "time_find_grandma",
    "time_finish_game",
    "player_health_end",
    "grandma_health_end",
    "priority_items_saved",
    "total_priority_items",
]

@app.route("/health", methods=["GET"])
def health():
    return jsonify({"status": "ok"})

@app.route("/predict-score", methods=["POST"])
def predict_score():
    data = request.get_json(force=True)
    print("[ScoreDebug] Incoming JSON:", data)

    try:
        features = [float(data[name]) for name in FEATURE_ORDER]
        print("[ScoreDebug] Features:", features)
    except KeyError as e:
        msg = f"missing field: {e.args[0]}"
        print("[ScoreDebug] Error:", msg)
        return jsonify({"error": msg}), 400
    except ValueError as e:
        msg = f"invalid value: {e}"
        print("[ScoreDebug] Error:", msg)
        return jsonify({"error": msg}), 400

    X = np.array([features])
    score = float(model.predict(X)[0])
    score = max(0.0, min(100.0, score))

    print("[ScoreDebug] Predicted score:", score)
    return jsonify({"score": score})

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000, debug=True)