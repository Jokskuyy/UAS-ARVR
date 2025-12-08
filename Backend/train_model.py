import joblib
import pandas as pd
from pathlib import Path
from sklearn.ensemble import RandomForestRegressor
from sklearn.model_selection import train_test_split
from sklearn.metrics import mean_absolute_error

# Use relative paths from the script location
DATA_PATH = Path(__file__).parent / "game_sessions.csv"
MODEL_PATH = Path(__file__).parent / "model.pkl"

def load_data():
    df = pd.read_csv(DATA_PATH)

    feature_cols = [
        "time_find_grandma",
        "time_finish_game",
        "player_health_end",
        "grandma_health_end",
        "priority_items_saved",
        "total_priority_items",
    ]
    target_col = "score"

    X = df[feature_cols]
    y = df[target_col]

    return X, y

def train():
    X, y = load_data()

    X_train, X_test, y_train, y_test = train_test_split(
        X, y, test_size=0.2, random_state=42
    )

    model = RandomForestRegressor(
        n_estimators=200,
        max_depth=8,
        random_state=42
    )

    model.fit(X_train, y_train)

    y_pred = model.predict(X_test)
    mae = mean_absolute_error(y_test, y_pred)
    print(f"MAE on test set: {mae:.2f}")

    MODEL_PATH.parent.mkdir(parents=True, exist_ok=True)
    joblib.dump(model, MODEL_PATH)
    print(f"Saved model to {MODEL_PATH}")

if __name__ == "__main__":
    train()