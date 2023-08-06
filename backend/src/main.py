from __future__ import annotations
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from pymongo import MongoClient
from pydantic import BaseModel
from typing import List, Optional


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


def jsonify_id(obj: dict):
    object_id = obj.pop("_id")
    obj["id"] = object_id.binary.hex()
    return obj


class Ingredient(BaseModel):
    quantity: float
    unit: str
    name: str

class PostRecipeRequest(BaseModel):
    name: str
    author: str
    tags: Optional[List[str]]
    ingredients: List[Ingredient]
    body: str

class GetRecipesRequest(BaseModel):
    name: Optional[str]
    author: Optional[str]
    tags: Optional[List[str]]
    ingredients: Optional[List[Ingredient]]
    body: Optional[str]

@app.post("/recipes")
async def post_recipes(request: GetRecipesRequest):
    print()
    return {
        "status": "success",
        "recipes": [jsonify_id(recipe) for recipe in recipes.find()],
    }

@app.post("/recipe")
async def post_recipe(request: PostRecipeRequest):
    if request.tags:
        tags = [tag.strip() for tag in request.tags]
    else:
        tags = []
    recipes.insert_one({
        "Name": request.name,
        "Author": request.author,
        "Tags": tags,
        "Ingredients": [ingredient.dict() for ingredient in request.ingredients],
        "Body": request.body,
    })
    return {"status": "success"}