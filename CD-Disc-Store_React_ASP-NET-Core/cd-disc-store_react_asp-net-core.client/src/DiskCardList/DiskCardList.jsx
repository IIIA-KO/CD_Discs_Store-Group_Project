// import React from "react";
// import DiskCard from "../DiskCard/DiskCard";
// import './DiskCardList.css'
// const DiskCardList = ({ data }) => {
//   const postCard = data.map((item) => {
//     const {id}=item
//     return <DiskCard item={item} key={id} />;
//   });

//   return <div className="Card-list">{postCard}</div>;
// };

// export default DiskCardList;
// Ваш родительский компонент, использующий DiskCard
import React from 'react';
import DiskCard from "../DiskCard/DiskCard";
import './DiskCardList.css'
const DiskCardList = ({ data, addToCart }) => {
  const postCard = data.map((item) => {
    const { id } = item;
    return <DiskCard item={item} key={id} addToCart={addToCart} />; // Передача addToCart в пропсы DiskCard
  });

  return <div className="Card-list">{postCard}</div>;
};

export default DiskCardList;
