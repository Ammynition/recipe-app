from __future__ import annotations
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

from pymongo import MongoClient
from pydantic import BaseModel
from typing import List, Optional
from bson import ObjectId

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

def bsonify_id(obj: str):
    return ObjectId(obj)

class Ingredient(BaseModel):
    quantity: float
    unit: str
    name: str

class PostRecipeRequest(BaseModel):
    name: str
    author: str
    ingredients: List[Ingredient]
    body: str
    tags: Optional[List[str]] = None

class GetRecipesRequest(BaseModel):
    query: Optional[str] = None

class DeleteRecipeRequest(BaseModel):
    recipe_id: str

class GetRecipeRequest(BaseModel):
    recipe_id: str


@app.post("/get-recipe")
async def get_recipe(request: GetRecipeRequest):
    return {
        "status": "success",
        "recipe": jsonify_id(recipes.find_one({"_id": bsonify_id(request.recipe_id)}))
    }


@app.post("/recipes")
async def post_recipes(request: GetRecipesRequest):
    print(request.query)
    return {
        "status": "success",
        #find recipes based on the filter, this filter says find all fields except for the body
        #this returns an iterable and we iterate over each recipe and search said recipe for the query term (ignore case)
        #if the recipe matches the query term then convert it to a json object and then convert it into a list of json objects
        "recipes": [jsonify_id(recipe) for recipe in recipes.find({}, {"Body": 0}) if search_db(request.query.lower(), recipe)],
    }

@app.post("/create-recipe")
async def post_recipe(request: PostRecipeRequest):
    if request.tags:
        #pull the tags out of the request object, loop over every single tag and remove the white space
        #add clean tag to list
        tags = [tag.strip() for tag in request.tags]
    else:
        tags = []
    recipes.insert_one({
        "Name": request.name,
        "Author": request.author,
        "Ingredients": [ingredient.dict() for ingredient in request.ingredients],
        "Body": request.body,
        "Tags": tags,
    })
    return {"status": "success"}

@app.post("/delete-recipe")
async def delete_recipes(request: DeleteRecipeRequest):
    if recipes.delete_one({"_id": bsonify_id(request.recipe_id)}).deleted_count == 0:
        return {"status": "error"}
    else:
        return {"status": "success"}

def search_db(query: str, recipe: dict) -> bool:
    if query in recipe["Name"].lower(): 
        return True
    if query in recipe["Author"].lower(): 
        return True
    for ingredient in recipe["Ingredients"]:
        if query in ingredient["name"].lower(): 
            return True
    if query in [tag.lower() for tag in recipe["Tags"]]: 
        return True
