import React from 'react';
import { useEffect, useState } from 'react'
import CardList from '../CardList/CardList';
import MovieSearch from '../../MovieSearch/MovieSearch';
import Pagination from '../../Pagination/Pagination';
const Films = () => {

  const [items, setItems] = useState([]);
  const [currentPage, setCurrentPage] = useState(0);

  useEffect(() => {
    const fetchData = async () => {
      const response = await fetch(`https://localhost:7117/Film/GetAll?skip=${currentPage * 10}`);
      const data = await response.json();
      setItems(data);
    };
    
    fetchData();
  }, [currentPage]);

  const handlePageClick = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  
   return(
    <div>
      <MovieSearch movies={items}  />
      {/*<CardList data={items} />*/}
      {/* Пагинация */}
      <Pagination
        currentPage={currentPage}
        totalPages={20}
        onPageClick={handlePageClick}
      />
    </div>
  );
}

export default Films;
