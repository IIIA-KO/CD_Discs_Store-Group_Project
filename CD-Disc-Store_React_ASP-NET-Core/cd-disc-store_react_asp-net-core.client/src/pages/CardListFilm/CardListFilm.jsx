import React from "react";
import Card from "../Card/Card";
import './CardListFilm.css'
const CardListFilm = ({ data }) => {
  const postCard = data.map((item) => {
    const {id}=item
    return <Card item={item} key={id} />;
  });

  return <div className="Card-list">{postCard}</div>;
};

export default CardListFilm;
