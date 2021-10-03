import React, { Component } from 'react';
import axios from 'axios';

class Fib extends Component {
  state = {
    seenIndexes: [],
    values: [],
    index: ''
  };

  componentDidMount() {
    this.fetchValues();
    this.fetchIndexes();
  }

  async fetchValues() {
    const values = await axios.get('/api/fibonacci/values');
    this.setState({ values: values.data.data });
  }

  async fetchIndexes() {
    const seenIndexes = await axios.get('/api/fibonacci/values');
    this.setState({
      seenIndexes: seenIndexes.data.data
    });
  }

  handleSubmit = async event => {
    event.preventDefault();

    await axios.post('/api/fibonacci', {
      index: this.state.index
    });
    this.setState({ index: '' });
  };

  renderSeenIndexes() {
    return this.state.seenIndexes.map(result => result.number).join(', ');
  }

  renderValues() {
    const entries = [];

    for (let result of this.state.values) {
      entries.push(
        <div key={result.number}>
          For index {result.number} I calculated {result.result}
        </div>
      );
    }

    return entries;
  }

  render() {
    return (
      <div>
        <form onSubmit={this.handleSubmit}>
          <label>Enter your index:</label>
          <input
            value={this.state.index}
            onChange={event => this.setState({ index: event.target.value })}
          />
          <button>Submit</button>
        </form>

        <h3>Indexes I have seen:</h3>
        {this.renderSeenIndexes()}

        <h3>Calculated Values:</h3>
        {this.renderValues()}
      </div>
    );
  }
}

export default Fib;