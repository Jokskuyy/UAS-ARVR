import csv
import random
from pathlib import Path

OUTPUT_PATH = Path(__file__).parent / "game_sessions.csv"
N_SAMPLES = 300  # jumlah data dummy

def generate_session():
    # parameter dasar
    max_time = 300  # 5 menit (detik)
    total_priority_items = random.randint(3, 5)

    # sebagian run "bagus", sebagian "buruk"
    # prob 0.4 run bagus, 0.4 menengah, 0.2 jelek
    r = random.random()
    if r < 0.4:  # bagus
        time_find = random.uniform(20, 80)
        time_finish = random.uniform(time_find + 30, min(time_find + 150, max_time))
        player_hp = random.uniform(70, 100)
        grandma_hp = random.uniform(70, 100)
        priority_saved = random.randint(total_priority_items - 1, total_priority_items)
    elif r < 0.8:  # menengah
        time_find = random.uniform(60, 180)
        time_finish = random.uniform(time_find + 60, min(time_find + 200, max_time + 30))
        player_hp = random.uniform(40, 80)
        grandma_hp = random.uniform(40, 80)
        priority_saved = random.randint(1, total_priority_items)
    else:  # jelek
        time_find = random.uniform(150, 280)
        time_finish = random.uniform(time_find + 60, max_time + 60)
        player_hp = random.uniform(5, 60)
        grandma_hp = random.uniform(0, 60)
        priority_saved = random.randint(0, total_priority_items - 1)

    # clamp ke max_time + sedikit toleransi
    time_find = min(time_find, max_time + 10)
    time_finish = min(time_finish, max_time + 60)

    # scoring rule sederhana untuk bikin label
    # base dari 100, lalu dikurang penalti
    score = 100.0

    # penalti waktu
    score -= (time_find / max_time) * 20.0        # maksimal -20
    score -= (time_finish / (max_time + 60)) * 25.0  # maksimal -25

    # penalti HP hilang
    avg_hp = (player_hp + grandma_hp) / 2.0  # 0–100
    score -= (100.0 - avg_hp) * 0.3          # maksimal -30

    # bonus item
    if total_priority_items > 0:
        ratio = priority_saved / total_priority_items
    else:
        ratio = 0.0
    score += ratio * 25.0                    # maksimal +25

    # penalti besar kalau lewat waktu
    if time_finish > max_time:
        score -= 20.0

    # clamp 0–100
    score = max(0.0, min(100.0, score))

    return {
        "time_find_grandma": round(time_find, 2),
        "time_finish_game": round(time_finish, 2),
        "player_health_end": round(player_hp, 1),
        "grandma_health_end": round(grandma_hp, 1),
        "priority_items_saved": priority_saved,
        "total_priority_items": total_priority_items,
        "score": round(score, 1),
    }

def main():
    fieldnames = [
        "time_find_grandma",
        "time_finish_game",
        "player_health_end",
        "grandma_health_end",
        "priority_items_saved",
        "total_priority_items",
        "score",
    ]

    rows = [generate_session() for _ in range(N_SAMPLES)]

    with open(OUTPUT_PATH, "w", newline="") as f:
        writer = csv.DictWriter(f, fieldnames=fieldnames)
        writer.writeheader()
        writer.writerows(rows)

    print(f"Wrote {len(rows)} samples to {OUTPUT_PATH}")

if __name__ == "__main__":
    main()