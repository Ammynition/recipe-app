from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from pymongo import MongoClient
from pydantic import BaseModel
from typing import List


app = FastAPI()


app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

client = MongoClient("mongodb://recipe_db:27017")
db = client.recipe_database
recipes = db.recipes

class PostRecipeRequest(BaseModel):
    name: str
    author: str
    tags: List[str]
    #ingredients: List[str]
    #body: str


@app.post("/recipe")
async def post_recipe(request: PostRecipeRequest):
    recipes.insert_one({
        "Name": request.name,
        "Author": request.author,
        "Tags": [tag.strip() for tag in request.tags],
        #"Ingredients": request.ingredients,
        #"Body": request.body,
    })
    return {"status": "success"}