import React from "react";
import Card from "../Card/Card";
import './CardList.css'
const CardList = ({ data }) => {
  const postCard = data.map((item) => {
    const {id}=item
    return <Card item={item} key={id} />;
  });

  return <div className="Card-list">{postCard}</div>;
};

export default CardList;
