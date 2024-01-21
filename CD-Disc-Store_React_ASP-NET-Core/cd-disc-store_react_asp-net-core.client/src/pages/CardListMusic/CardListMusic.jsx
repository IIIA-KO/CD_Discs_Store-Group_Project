import React from "react";
import CardMusic from "../CardMusic/CardMusic";
import './CardListMusic.css'
const CardList = ({ data }) => {
  const postCard = data.map((item) => {
    const {id}=item
    return <CardMusic item={item} key={id} />;
  });

  return <div className="Card-list">{postCard}</div>;
};

export default CardList;
