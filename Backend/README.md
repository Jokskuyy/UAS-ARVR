# Backend - ML Model Training & Deployment

This folder contains the machine learning model training and Flask API for score prediction.

## Setup

### 1. Create Virtual Environment

```bash
cd Backend
python -m venv venv
```

### 2. Activate Virtual Environment

**Windows (PowerShell):**

```powershell
.\venv\Scripts\Activate.ps1
```

**Windows (CMD):**

```cmd
.\venv\Scripts\activate.bat
```

**macOS/Linux:**

```bash
source venv/bin/activate
```

### 3. Install Dependencies

```bash
pip install -r requirements.txt
```

## Usage

### Generate Training Data

Generate dummy game session data for training:

```bash
python generate_dummy_data.py
```

This creates `game_sessions.csv` with 300 sample records.

### Train Model

Train the Random Forest model:

```bash
python train_model.py
```

This will:

- Load data from `game_sessions.csv`
- Train a Random Forest Regressor
- Evaluate on test set (MAE metric)
- Save model to `model.pkl`

### Run Flask API

Start the Flask server for score prediction:

```bash
python app.py
```

API will be available at `http://127.0.0.1:5000`

## API Endpoints

### Health Check

```http
GET /health
```

**Response:**

```json
{
  "status": "ok"
}
```

### Predict Score

```http
POST /predict-score
Content-Type: application/json

{
  "time_find_grandma": 45.5,
  "time_finish_game": 180.0,
  "player_health_end": 85.0,
  "grandma_health_end": 90.0,
  "priority_items_saved": 4,
  "total_priority_items": 5
}
```

**Response:**

```json
{
  "score": 78.5
}
```

## Model Features

The model uses the following features for prediction:

1. **time_find_grandma** - Time taken to find grandma (seconds)
2. **time_finish_game** - Total time to complete game (seconds)
3. **player_health_end** - Player health at end (0-100)
4. **grandma_health_end** - Grandma health at end (0-100)
5. **priority_items_saved** - Number of priority items saved
6. **total_priority_items** - Total number of priority items

**Target:** `score` (0-100)

## Files

- `app.py` - Flask API server
- `train_model.py` - Model training script
- `generate_dummy_data.py` - Dummy data generator
- `requirements.txt` - Python dependencies
- `game_sessions.csv` - Training data (generated)
- `model.pkl` - Trained model (gitignored)

## Notes

- The trained model (`model.pkl`) is not committed to Git (see `.gitignore`)
- You must train the model locally before running the API
- Virtual environment (`venv/`) is also gitignored
