import React, { useState, useEffect } from 'react';
import "./DiskSearch.css"
import DiskCardList from '../DiskCardList/DiskCardList';
import Cart from '../pages/Cart/Cart';

const DiskSearch = ({discs, currentPage, itemsPerPage, onUpdateTotalPages}) => {
  const [searchText, setSearchText] = useState('');
  const [cartItems, setCartItems] = useState([]);
  const [searchResults, setSearchResults] = useState([]);
  const [totalItems, setTotalItems] = useState(0);

  useEffect(() => {
    const fetchSearchResults = async () => {
      try {
        const response = await fetch(`https://localhost:7117/Discs/GetAll?searchText=${searchText}&skip=${(currentPage - 1) * itemsPerPage}&take=${itemsPerPage}`);
        const data = await response.json();
        setSearchResults(data.items);
        setTotalItems(data.countItems);
        onUpdateTotalPages(Math.ceil(data.countItems / itemsPerPage));
      } catch (error) {
        console.error(error);
      }
    };

    fetchSearchResults();
  }, [searchText, discs]);

  const handleSearchChange = (e) => {
    setSearchText(e.target.value);
  };

  const addToCart = (item) => {
    console.log(item, 'add to cart');
    setCartItems([...cartItems, item]);
  };

  return (
    <div>
      <div className='search_bar'>
        <input type="text" placeholder='Search Disk' value={searchText} onChange={handleSearchChange} />
      </div>
      <ul>
        <DiskCardList data={searchResults} addToCart={addToCart} />
        <Cart items={cartItems} />
      </ul>
    </div>
  );
};

export default DiskSearch;