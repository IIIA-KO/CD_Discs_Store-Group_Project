import React from 'react'
import { useEffect, useState } from 'react'
import CardListMusic from '../CardListMusic/CardListMusic';
import MusicSearch from '../../MusicSearch/MusicSearch';
import Pagination from '../../Pagination/Pagination';
const Music = () => {
  // const [items, setItems] = useState([]);

  // useEffect(() => {

  //   fetch("https://localhost:7117/Music/GetAll?skip=0").then(res => res.json()).then(data => setItems(data)).catch(error => console.error(error));
  // }, [])
  const [items, setItems] = useState([]);
  const [currentPage, setCurrentPage] = useState(0);

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch(`https://localhost:7117/Music/GetAll?skip=${currentPage * 10}`);
      const data = await response.json();
      setItems(data);
    };
    
    fetchData();
  }, [currentPage]);

  const handlePageClick = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  return (
    <div>
      <MusicSearch musics={items}/>
      {/*<CardListMusic data={items} />*/}
      {/* Пагинация */}
      <Pagination
        currentPage={currentPage}
        totalPages={4}
        onPageClick={handlePageClick}
      />
    </div>
  )
}

export default Music

