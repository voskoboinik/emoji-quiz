import pandas as pd

emoji = pd.read_csv("movies_emojis.csv", encoding="utf-8")
emoji = emoji.rename(columns={"Movies": "title_en", "Emojis": "emoji"})
print("Фильмов с эмодзи:", len(emoji))

basics = pd.read_csv(
    "title.basics.tsv", sep="\t", na_values="\\N", quoting=3, dtype=str,
    encoding="utf-8", usecols=["tconst", "titleType", "primaryTitle"]
)
movies = basics[basics["titleType"] == "movie"]
print("Всего записей в basics:", len(basics), "| из них фильмов:", len(movies))

merged = emoji.merge(
    movies, left_on="title_en", right_on="primaryTitle", how="inner"
)
merged = merged.drop_duplicates(subset="title_en")
print("Нашли код для фильмов:", len(merged))

parts = []
for chunk in pd.read_csv(
    "title.akas.tsv", sep="\t", na_values="\\N", quoting=3, dtype=str,
    encoding="utf-8", usecols=["titleId", "title", "region"], chunksize=2_000_000
):
    parts.append(chunk[chunk["region"] == "RU"])
ru = pd.concat(parts)
ru = ru.drop_duplicates(subset="titleId")
ru = ru.rename(columns={"titleId": "tconst", "title": "title_ru"})
print("Русских названий нашли:", len(ru))

result = merged.merge(ru, on="tconst", how="inner")

final = result[["tconst", "title_ru", "emoji"]].dropna()
final.to_csv("movies_ru_emoji.tsv", sep="\t", index=False, encoding="utf-8")

print("ИТОГО строк в результате:", len(final))
print(final.head())